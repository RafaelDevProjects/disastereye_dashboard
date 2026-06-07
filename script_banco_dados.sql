-- ================================================================
-- DisasterEye — Script do Banco de Dados MySQL
-- Disciplina: C# Development · FIAP · Turma: 3ESPR
-- Tema: A Nova Economia Espacial — Monitoramento de Desastres
-- ================================================================

CREATE DATABASE IF NOT EXISTS disastereye_db
    CHARACTER SET utf8mb4
    COLLATE utf8mb4_unicode_ci;

USE disastereye_db;

-- ── Categorias de Impacto ────────────────────────────────────────
CREATE TABLE IF NOT EXISTS CategoriasImpacto (
    Id            INT          NOT NULL AUTO_INCREMENT,
    Nome          VARCHAR(100) NOT NULL,
    Descricao     VARCHAR(500) NOT NULL DEFAULT '',
    TipoDesastre  VARCHAR(100) NOT NULL DEFAULT '',
    PRIMARY KEY (Id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ── Usuários ─────────────────────────────────────────────────────
CREATE TABLE IF NOT EXISTS Usuarios (
    Id           INT          NOT NULL AUTO_INCREMENT,
    Nome         VARCHAR(100) NOT NULL,
    Email        VARCHAR(200) NOT NULL,
    SenhaHash    VARCHAR(255) NOT NULL,   -- BCrypt hash, NUNCA texto limpo
    Perfil       VARCHAR(50)  NOT NULL DEFAULT 'Pesquisador',
    DataCadastro DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    Ativo        TINYINT(1)   NOT NULL DEFAULT 1,
    PRIMARY KEY (Id),
    UNIQUE KEY UQ_Usuarios_Email (Email)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ── Tecnologias ──────────────────────────────────────────────────
CREATE TABLE IF NOT EXISTS Tecnologias (
    Id                INT           NOT NULL AUTO_INCREMENT,
    Nome              VARCHAR(200)  NOT NULL,
    Descricao         VARCHAR(1000) NOT NULL DEFAULT '',
    OrigemEspacial    VARCHAR(200)  NOT NULL DEFAULT '',
    Latitude          DOUBLE        NULL,
    Longitude         DOUBLE        NULL,
    Status            VARCHAR(50)   NOT NULL DEFAULT 'Ativo',
    DataCadastro      DATETIME      NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CategoriaImpactoId INT          NOT NULL,
    PRIMARY KEY (Id),
    CONSTRAINT FK_Tecnologias_Categorias
        FOREIGN KEY (CategoriaImpactoId) REFERENCES CategoriasImpacto (Id)
        ON DELETE RESTRICT ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ── Histórico de Migrations do EF Core ───────────────────────────
CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId`    VARCHAR(150) NOT NULL,
    `ProductVersion` VARCHAR(32)  NOT NULL,
    PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ================================================================
-- SEED — Dados Iniciais
-- ================================================================

-- Categorias de Impacto
INSERT INTO CategoriasImpacto (Id, Nome, Descricao, TipoDesastre) VALUES
(1, 'Incêndios Florestais', 'Monitoramento de incêndios via satélite',          'Wildfires'),
(2, 'Inundações',           'Detecção de cheias e alagamentos',                  'Floods'),
(3, 'Terremotos',           'Análise sísmica via sensores espaciais',             'Earthquakes'),
(4, 'Vulcões',              'Monitoramento de atividade vulcânica',               'Volcanoes'),
(5, 'Tempestades Severas',  'Rastreamento de furacões e tempestades',             'Severe Storms')
ON DUPLICATE KEY UPDATE Nome = VALUES(Nome);

-- Administrador padrão
-- Senha: Admin@123 (hash BCrypt gerado pelo EF Seed — será sobrescrito na migration)
INSERT INTO Usuarios (Id, Nome, Email, SenhaHash, Perfil, DataCadastro, Ativo)
VALUES (
    1,
    'Administrador',
    'admin@disastereye.com',
    '$2a$11$placeholder_hash_will_be_set_by_migration',
    'Administrador',
    '2025-01-01 00:00:00',
    1
)
ON DUPLICATE KEY UPDATE Email = VALUES(Email);

-- Tecnologias de exemplo
INSERT INTO Tecnologias
    (Id, Nome, Descricao, OrigemEspacial, Latitude, Longitude, Status, DataCadastro, CategoriaImpactoId)
VALUES
(1,
 'EONET Fire Tracker',
 'Sistema de rastreamento de eventos de incêndio usando dados da API EONET da NASA. Utiliza coordenadas de satélite para identificar focos ativos em tempo real.',
 'NASA EONET', -15.78, -47.93, 'Ativo', '2025-01-01 00:00:00', 1),

(2,
 'Sentinel Flood Monitor',
 'Monitoramento de inundações por imagens do satélite Sentinel-1 com tecnologia SAR (Synthetic Aperture Radar) da Agência Espacial Europeia.',
 'ESA Sentinel-1', -23.55, -46.63, 'Monitorando', '2025-01-01 00:00:00', 2),

(3,
 'GPS Seismic Array',
 'Rede de sensores sísmicos derivados da tecnologia GPS espacial para detecção precoce de terremotos em regiões vulneráveis da Amazônia.',
 'GPS/GNSS Satellites', -3.10, -60.02, 'Alerta', '2025-01-01 00:00:00', 3),

(4,
 'GOES-16 Storm Eye',
 'Rastreamento de tempestades severas e furacões no Atlântico Sul via satélite geoestacionário GOES-16 da NOAA.',
 'NOAA GOES-16', -10.00, -35.00, 'Monitorando', '2025-01-01 00:00:00', 5),

(5,
 'Volcano SAR Monitor',
 'Monitoramento de deformações do solo em vulcões ativos brasileiros usando interferometria SAR de satélites Sentinel-2.',
 'ESA Sentinel-2', -22.44, -45.85, 'Ativo', '2025-01-01 00:00:00', 4)
ON DUPLICATE KEY UPDATE Nome = VALUES(Nome);

-- ================================================================
-- VIEWS úteis para consultas
-- ================================================================

CREATE OR REPLACE VIEW vw_TecnologiasPorCategoria AS
SELECT
    c.Nome         AS Categoria,
    c.TipoDesastre AS TipoDesastre,
    COUNT(t.Id)    AS TotalTecnologias
FROM CategoriasImpacto c
LEFT JOIN Tecnologias t ON t.CategoriaImpactoId = c.Id
GROUP BY c.Id, c.Nome, c.TipoDesastre;

CREATE OR REPLACE VIEW vw_EstatisticasDashboard AS
SELECT
    (SELECT COUNT(*) FROM Tecnologias)                             AS TotalTecnologias,
    (SELECT COUNT(DISTINCT OrigemEspacial) FROM Tecnologias)       AS TotalMissoesMapeadas,
    (SELECT COUNT(*) FROM CategoriasImpacto)                       AS TotalSetoresBeneficiados,
    (SELECT COUNT(*) FROM Tecnologias WHERE Status IN ('Alerta','Crítico')) AS AlertasAtivos;

-- ================================================================
-- FIM DO SCRIPT
-- ================================================================
