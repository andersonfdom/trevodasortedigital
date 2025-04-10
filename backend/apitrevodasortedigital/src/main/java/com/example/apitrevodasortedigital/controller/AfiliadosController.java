package com.example.apitrevodasortedigital.controller;

import com.example.apitrevodasortedigital.DTO.AfiliadosDTO;
import com.example.apitrevodasortedigital.DTO.LoginDTO;
import com.example.apitrevodasortedigital.Utils;
import com.example.apitrevodasortedigital.dao.AfiliadosDao;
import com.example.apitrevodasortedigital.model.Afiliados;
import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.Parameter;
import io.swagger.v3.oas.annotations.responses.ApiResponse;
import io.swagger.v3.oas.annotations.responses.ApiResponses;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.time.LocalDateTime;
import java.util.List;
import java.util.UUID;

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
    public Afiliados createAfiliados(@io.swagger.v3.oas.annotations.parameters.RequestBody(description = "Dados do afiliado a ser criado") @RequestBody AfiliadosDTO afiliadosDTO) throws Exception {
        Afiliados afiliados = new Afiliados();

        afiliados.setId(0);
        afiliados.setNome(afiliadosDTO.getNome());
        afiliados.setTelefone(afiliadosDTO.getTelefone());
        afiliados.setEmail(afiliadosDTO.getEmail());
        afiliados.setCpf(afiliadosDTO.getCpf());
        afiliados.setCodafiliado(afiliadosDTO.getCodafiliado());
        afiliados.setSenha(utils.criptografar(afiliadosDTO.getSenha()));
        afiliados.setUsuariologado(0);
        afiliados.setAtivo(0);

        return afiliadosRepository.save(afiliados);
    }

    @PutMapping("/{id}")
    public Afiliados updateAfiliados(@PathVariable Integer id, @RequestBody AfiliadosDTO afiliadosDTO) throws Exception {
        Afiliados afiliados = new Afiliados();

        afiliados.setId(afiliadosDTO.getId());
        afiliados.setNome(afiliadosDTO.getNome());
        afiliados.setTelefone(afiliadosDTO.getTelefone());
        afiliados.setEmail(afiliadosDTO.getEmail());
        afiliados.setCpf(afiliadosDTO.getCpf());
        afiliados.setCodafiliado(afiliadosDTO.getCodafiliado());
        afiliados.setSenha(utils.criptografar(afiliadosDTO.getSenha()));

        return afiliadosRepository.save(afiliados);
    }

    @DeleteMapping("/{id}")
    public void deleteAfiliados(@PathVariable Integer id) {
        afiliadosRepository.deleteById(id);
    }

    @Operation(summary = "Se o login senha for válido, retorna o Token")
    @ApiResponse(responseCode = "201", description = "Afiliado logado com sucesso")
    @PostMapping("/retornarTokenLogin")
    public String retornarTokenLogin(@RequestBody LoginDTO loginDTO) throws Exception {
        String token = "";
        String senhaCriptografada = utils.criptografar(loginDTO.getSenha());
        Afiliados afiliados = afiliadosRepository.findByEmail(loginDTO.getLogin());

        if (afiliados != null && afiliados.getSenha().equals(senhaCriptografada)) {
            token = UUID.randomUUID().toString().replaceAll("-", "").substring(0, 24);

            afiliados.setToken(token);
            afiliados.setUsuariologado(1);
            afiliados.setUltimoacesso(LocalDateTime.now());

            afiliadosRepository.save(afiliados);

            return token;
        }else{
            return "Usuário e/ou senha inválidos.";
        }
    }

    @Operation(summary = "Se o token for válido, realiza o logoff")
    @ApiResponse(responseCode = "201", description = "Logoff realizado com sucesso.")
    @PostMapping("/realizarLogoff/{token}")
    public void realizarLogoff(@PathVariable String token) {
        Afiliados afiliados = afiliadosRepository.findByToken(token);

        if (afiliados == null) return;

        afiliados.setToken("");
        afiliados.setUsuariologado(0);

        afiliadosRepository.save(afiliados);
    }
}