#!/bin/bash

CONTAINER_NAME="tracklet-db"
POSTGRES_USER="tracklet"
POSTGRES_PASSWORD="@myStrongPassword"
POSTGRES_DB="tracklet"
PORT=5432
DOCKER_IMAGE="postgres:latest"
VOLUME_NAME="tracklet_data"

MIGRATION_USER="mtracklet"
MIGRATION_PASSWORD="@myStrongPassword"
APP_USER="dtracklet"
APP_PASSWORD="@myStrongPassword"

check_docker() {
    if ! command -v docker &>/dev/null; then
        echo "❌ Docker não encontrado"
        exit 1
    fi

    if ! docker info &>/dev/null; then
        echo "❌ Docker não está rodando"
        exit 1
    fi
}

wait_for_postgres() {
    local container_id=$1
    local max_attempts=15
    local attempt=0
    
    echo "⏳ Aguardando PostgreSQL inicializar..."
    
    until docker exec $container_id pg_isready -U $POSTGRES_USER &>/dev/null; do
        attempt=$((attempt + 1))
        if [ $attempt -ge $max_attempts ]; then
            echo "❌ PostgreSQL não iniciou após $max_attempts tentativas"
            exit 1
        fi
        sleep 2
    done
}

create_db_users() {
    local container_id=$1
    
    echo "👥 Configurando permissões universais..."
    
    SQL_COMMANDS=$(cat <<EOF

CREATE SCHEMA IF NOT EXISTS public;
GRANT ALL ON SCHEMA public TO ${POSTGRES_USER};

DO \$\$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_roles WHERE rolname = '${MIGRATION_USER}') THEN
        CREATE ROLE ${MIGRATION_USER} WITH LOGIN PASSWORD '${MIGRATION_PASSWORD}';
    END IF;
    
    IF NOT EXISTS (SELECT 1 FROM pg_roles WHERE rolname = '${APP_USER}') THEN
        CREATE ROLE ${APP_USER} WITH LOGIN PASSWORD '${APP_PASSWORD}';
    END IF;
    
    ALTER ROLE ${MIGRATION_USER} WITH CREATEDB CREATEROLE;
    ALTER ROLE ${MIGRATION_USER} SET search_path TO public;
    ALTER ROLE ${APP_USER} SET search_path TO public;
END
\$\$;

GRANT ALL PRIVILEGES ON DATABASE ${POSTGRES_DB} TO ${MIGRATION_USER};
GRANT ALL PRIVILEGES ON SCHEMA public TO ${MIGRATION_USER};
GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO ${MIGRATION_USER};
GRANT ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA public TO ${MIGRATION_USER};
GRANT ALL PRIVILEGES ON ALL FUNCTIONS IN SCHEMA public TO ${MIGRATION_USER};

ALTER DEFAULT PRIVILEGES 
    FOR ROLE ${MIGRATION_USER}
    IN SCHEMA public
    GRANT SELECT, INSERT, UPDATE, DELETE ON TABLES TO ${APP_USER};

ALTER DEFAULT PRIVILEGES 
    FOR ROLE ${MIGRATION_USER}
    IN SCHEMA public
    GRANT SELECT, USAGE ON SEQUENCES TO ${APP_USER};

ALTER DEFAULT PRIVILEGES 
    FOR ROLE ${MIGRATION_USER}
    IN SCHEMA public
    GRANT EXECUTE ON FUNCTIONS TO ${APP_USER};

DO \$\$
DECLARE
    r RECORD;
BEGIN
    FOR r IN 
        SELECT table_name 
        FROM information_schema.tables 
        WHERE table_schema = 'public'
    LOOP
        IF r.table_name = 'VersionInfo' THEN
            EXECUTE format('GRANT SELECT ON TABLE public.%I TO %I', r.table_name, '${APP_USER}');
        ELSE
            EXECUTE format('GRANT SELECT, INSERT, UPDATE, DELETE ON TABLE public.%I TO %I', r.table_name, '${APP_USER}');
        END IF;
    END LOOP;
END
\$\$;

CREATE OR REPLACE FUNCTION public.grant_app_permissions()
RETURNS event_trigger AS \$\$
BEGIN
    EXECUTE format('GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA public TO %I', '${APP_USER}');
    EXECUTE format('GRANT SELECT ON TABLE public."VersionInfo" TO %I', '${APP_USER}');
END;
\$\$ LANGUAGE plpgsql;

DO \$\$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_event_trigger WHERE evtname = 'trg_grant_permissions') THEN
        CREATE EVENT TRIGGER trg_grant_permissions
        ON ddl_command_end
        WHEN TAG IN ('CREATE TABLE', 'CREATE TABLE AS')
        EXECUTE FUNCTION public.grant_app_permissions();
    END IF;
END
\$\$;
EOF
    )

    docker exec -i $container_id psql -U $POSTGRES_USER -d $POSTGRES_DB <<< "$SQL_COMMANDS"
    
    echo "✅ Permissões configuradas para todas as tabelas (atuais e futuras)"
}

create_postgres_container() {
    if ! docker volume inspect $VOLUME_NAME &>/dev/null; then
        echo "💾 Criando volume Docker..."
        docker volume create $VOLUME_NAME
    fi

    echo "🐘 Iniciando PostgreSQL container..."
    docker run -d \
        --name $CONTAINER_NAME \
        -e POSTGRES_USER=$POSTGRES_USER \
        -e POSTGRES_PASSWORD=$POSTGRES_PASSWORD \
        -e POSTGRES_DB=$POSTGRES_DB \
        -p $PORT:5432 \
        -v $VOLUME_NAME:/var/lib/postgresql/data \
        $DOCKER_IMAGE

    container_id=$(docker ps -qf "name=$CONTAINER_NAME")
    
    if [ -z "$container_id" ]; then
        echo "❌ Falha ao iniciar container"
        exit 1
    fi

    wait_for_postgres $container_id
    
    echo "✅ Container pronto"
    create_db_users $container_id
    
    echo ""
    echo "🔌 INFORMAÇÕES DE CONEXÃO"
    echo "=========================="
    printf "%-12s %s\n" "Host:" "localhost"
    printf "%-12s %s\n" "Porta:" "$PORT"
    printf "%-12s %s\n" "Database:" "$POSTGRES_DB"
    echo ""
    printf "%-12s %s\n" "Admin:" "$POSTGRES_USER / $POSTGRES_PASSWORD"
    printf "%-12s %s\n" "Migração:" "$MIGRATION_USER / $MIGRATION_PASSWORD"
    printf "%-12s %s\n" "Aplicação:" "$APP_USER / $APP_PASSWORD"
    echo ""
    echo "🛑 Use 'docker stop $CONTAINER_NAME' para parar o container"
}

main() {
    check_docker

    if docker ps -a --format '{{.Names}}' | grep -q "^$CONTAINER_NAME$"; then
        echo "ℹ️  Container já existe"
        read -p "🔁 Recriar? (s/n) " -n 1 -r
        echo
        if [[ $REPLY =~ ^[Ss]$ ]]; then
            echo "🗑️  Removendo container existente..."
            docker rm -f $CONTAINER_NAME
            create_postgres_container
        else
            echo "⚡ Iniciando container existente..."
            docker start $CONTAINER_NAME
        fi
    else
        create_postgres_container
    fi
}

main