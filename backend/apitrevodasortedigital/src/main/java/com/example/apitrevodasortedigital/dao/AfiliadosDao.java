package com.example.apitrevodasortedigital.dao;

import com.example.apitrevodasortedigital.model.Afiliados;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface AfiliadosDao extends JpaRepository<Afiliados, Integer> {
    Afiliados findByEmail(String email);
    Afiliados findByToken(String token);
}