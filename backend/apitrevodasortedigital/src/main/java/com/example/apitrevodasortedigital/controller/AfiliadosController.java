package com.example.apitrevodasortedigital.controller;

import com.example.apitrevodasortedigital.Utils;
import com.example.apitrevodasortedigital.dao.AfiliadosDao;
import com.example.apitrevodasortedigital.model.Afiliados;
import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.Parameter;
import io.swagger.v3.oas.annotations.responses.ApiResponse;
import io.swagger.v3.oas.annotations.responses.ApiResponses;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;
import java.util.List;

@RestController
@RequestMapping("/api/afiliados")
public class AfiliadosController {

    @Autowired
    private AfiliadosDao afiliadosRepository;

    Utils utils = new Utils();

    @Operation(summary = "Listar todos os afiliados")
    @ApiResponse(responseCode = "200", description = "Lista de afiliados encontrada")
    @GetMapping
    public List<Afiliados> getAllAfiliados() {
        return afiliadosRepository.findAll();
    }

    @Operation(summary = "Buscar afiliado por ID")
    @ApiResponses(value = {
            @ApiResponse(responseCode = "200", description = "Afiliado encontrado"),
            @ApiResponse(responseCode = "404", description = "Afiliado não encontrado")
    })
    @GetMapping("/{id}")
    public Afiliados getAfiliadosById(@Parameter(description = "ID do afiliado a ser buscado") @PathVariable Integer id) {
        return afiliadosRepository.findById(id).orElse(null);
    }

    @Operation(summary = "Criar um novo afiliado")
    @ApiResponse(responseCode = "201", description = "Afiliado criado com sucesso")
    @PostMapping
    public Afiliados createAfiliados(@io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Dados do afiliado a ser criado") @RequestBody Afiliados afiliados) throws Exception {
        String senha = utils.criptografar(afiliados.getSenha());
        afiliados.setSenha(senha);

        return afiliadosRepository.save(afiliados);
    }

    @PutMapping("/{id}")
    public Afiliados updateAfiliados(@PathVariable Integer id, @RequestBody Afiliados afiliados) throws Exception {
        afiliados.setId(id);

        String senha = utils.criptografar(afiliados.getSenha());
        afiliados.setSenha(senha);

        return afiliadosRepository.save(afiliados);
    }

    @DeleteMapping("/{id}")
    public void deleteAfiliados(@PathVariable Integer id) {
        afiliadosRepository.deleteById(id);
    }

    // ... outros métodos ...
}