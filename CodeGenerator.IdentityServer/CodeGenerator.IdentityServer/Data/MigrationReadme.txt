
ASP.Net Core Identity EF - DB Migrations Steps

Add-Migration InitialIdentityUserStoreDbMigration -Context ApplicationDbContext -OutputDir Data/Migrations/Identity/ApplicationDb
update-database -Context ApplicationDbContext






IdentityServer4 EF - DB Migrations Steps

Add-Migration InitialIdentityServerPersistedGrantDbMigration -Context PersistedGrantDbContext -o Data/Migrations/IdentityServer/PersistedGrantDb
update-database -Context PersistedGrantDbContext

Add-Migration InitialIdentityServerConfigurationDbMigration -Context ConfigurationDbContext -o Data/Migrations/IdentityServer/ConfigurationDb
update-database -Context ConfigurationDbContext