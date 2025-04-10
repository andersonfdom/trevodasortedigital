package com.example.apitrevodasortedigital.model;

import jakarta.persistence.*;
import lombok.Data;
import java.time.LocalDateTime;

@Entity
@Table(name = "afiliados")
@Data // Lombok para gerar getters, setters, toString, etc.
public class Afiliados {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer id;

    private String nome;
    private String telefone;
    private String email;
    private String cpf;
    private String codafiliado;
    private String senha;
    private Integer usuariologado;
    private LocalDateTime ultimoacesso;
    private String serialrecovery;
    private String token;
    private Integer ativo;

    // Você pode adicionar relacionamentos com outras entidades aqui
    // (por exemplo, se um afiliado tiver um relacionamento com apostas)
}