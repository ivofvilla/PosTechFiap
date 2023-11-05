-- Tabela "AspNetRoles"
CREATE TABLE "AspNetRoles" (
    "Id" TEXT NOT NULL,
    "Name" CHARACTER VARYING(256) CHECK (LENGTH("Name") <= 256),
    "NormalizedName" CHARACTER VARYING(256) CHECK (LENGTH("NormalizedName") <= 256),
    "ConcurrencyStamp" TEXT,
    PRIMARY KEY ("Id")
);

-- Tabela "AspNetUsers"
CREATE TABLE "AspNetUsers" (
    "Id" TEXT NOT NULL,
    "UserName" CHARACTER VARYING(256) CHECK (LENGTH("UserName") <= 256),
    "NormalizedUserName" CHARACTER VARYING(256) CHECK (LENGTH("NormalizedUserName") <= 256),
    "Email" CHARACTER VARYING(256) CHECK (LENGTH("Email") <= 256),
    "NormalizedEmail" CHARACTER VARYING(256) CHECK (LENGTH("NormalizedEmail") <= 256),
    "EmailConfirmed" BOOLEAN NOT NULL,
    "PasswordHash" TEXT,
    "SecurityStamp" TEXT,
    "ConcurrencyStamp" TEXT,
    "PhoneNumber" TEXT,
    "PhoneNumberConfirmed" BOOLEAN NOT NULL,
    "TwoFactorEnabled" BOOLEAN NOT NULL,
    "LockoutEnd" TIMESTAMP WITH TIME ZONE,
    "LockoutEnabled" BOOLEAN NOT NULL,
    "AccessFailedCount" INTEGER NOT NULL,
    PRIMARY KEY ("Id")
);

-- Tabela "AspNetRoleClaims"
CREATE TABLE "AspNetRoleClaims" (
    "Id" SERIAL NOT NULL,
    "RoleId" TEXT NOT NULL,
    "ClaimType" TEXT,
    "ClaimValue" TEXT,
    PRIMARY KEY ("Id"),
    CONSTRAINT "FK_AspNetRoleClaims_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE
);

-- Tabela "AspNetUserClaims"
CREATE TABLE "AspNetUserClaims" (
    "Id" SERIAL NOT NULL,
    "UserId" TEXT NOT NULL,
    "ClaimType" TEXT,
    "ClaimValue" TEXT,
    PRIMARY KEY ("Id"),
    CONSTRAINT "FK_AspNetUserClaims_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

-- Tabela "AspNetUserLogins"
CREATE TABLE "AspNetUserLogins" (
    "LoginProvider" TEXT NOT NULL,
    "ProviderKey" TEXT NOT NULL,
    "ProviderDisplayName" TEXT,
    "UserId" TEXT NOT NULL,
    PRIMARY KEY ("LoginProvider", "ProviderKey"),
    CONSTRAINT "FK_AspNetUserLogins_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

-- Tabela "AspNetUserRoles"
CREATE TABLE "AspNetUserRoles" (
    "UserId" TEXT NOT NULL,
    "RoleId" TEXT NOT NULL,
    PRIMARY KEY ("UserId", "RoleId"),
    CONSTRAINT "FK_AspNetUserRoles_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_AspNetUserRoles_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

-- Tabela "AspNetUserTokens"
CREATE TABLE "AspNetUserTokens" (
    "UserId" TEXT NOT NULL,
    "LoginProvider" TEXT NOT NULL,
    "Name" TEXT NOT NULL,
    "Value" TEXT,
    PRIMARY KEY ("UserId", "LoginProvider", "Name"),
    CONSTRAINT "FK_AspNetUserTokens_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id") ON DELETE CASCADE
);

-- Índices
CREATE INDEX "IX_AspNetRoleClaims_RoleId" ON "AspNetRoleClaims" ("RoleId");
CREATE UNIQUE INDEX "RoleNameIndex" ON "AspNetRoles" ("NormalizedName");
CREATE INDEX "IX_AspNetUserClaims_UserId" ON "AspNetUserClaims" ("UserId");
CREATE INDEX "IX_AspNetUserLogins_UserId" ON "AspNetUserLogins" ("UserId");
CREATE INDEX "IX_AspNetUserRoles_RoleId" ON "AspNetUserRoles" ("RoleId");
CREATE UNIQUE INDEX "EmailIndex" ON "AspNetUsers" ("NormalizedEmail");
CREATE UNIQUE INDEX "UserNameIndex" ON "AspNetUsers" ("NormalizedUserName");
