package main

import (
	"log"

	"github.com/JVitoroliv3ira/tracklet/internal/router"
)

func main() {
	r := router.SetupRouter()
	if err := r.Run(); err != nil {
		log.Fatalf("Erro ao iniciar o servidor: %v", err)
	}
}
