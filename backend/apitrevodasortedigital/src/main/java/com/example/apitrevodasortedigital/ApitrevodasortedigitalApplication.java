package com.example.apitrevodasortedigital;

import io.swagger.v3.oas.annotations.OpenAPIDefinition;
import io.swagger.v3.oas.annotations.info.Info;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;

@SpringBootApplication
@OpenAPIDefinition(
        info = @Info(
                title = "API Trevo da Sorte Digital",
                version = "v1",
                description = "API para gerenciamento de apostas e afiliados"
                // Você pode adicionar outras informações como contato, licença, etc.
        )
)
public class ApitrevodasortedigitalApplication {

    public static void main(String[] args) {
        SpringApplication.run(ApitrevodasortedigitalApplication.class, args);
    }
}