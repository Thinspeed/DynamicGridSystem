DO $$ 
DECLARE
    migrator_user TEXT := 'migrator';
    migrator_password TEXT := 'migrator_password';

    readonly_user TEXT := 'readonly';
    readonly_password TEXT := 'readonly_password';

    readwrite_user TEXT := 'readwrite';
    readwrite_password TEXT := 'readwrite_password';
BEGIN
    EXECUTE format('CREATE USER %I WITH PASSWORD %L', migrator_user, migrator_password);
    EXECUTE format('GRANT ALL PRIVILEGES ON DATABASE %I TO %I', current_database(), migrator_user);

    EXECUTE format('ALTER ROLE %I SET search_path TO public', migrator_user);
    EXECUTE format('GRANT USAGE, CREATE ON SCHEMA public TO %I', migrator_user);
    EXECUTE format('GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO %I', migrator_user);
    EXECUTE format('GRANT ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA public TO %I', migrator_user);
    EXECUTE format('ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT ALL ON TABLES TO %I', migrator_user);
    EXECUTE format('ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT ALL ON SEQUENCES TO %I', migrator_user);

    EXECUTE format('CREATE USER %I WITH PASSWORD %L', readonly_user, readonly_password);
    EXECUTE format('GRANT CONNECT ON DATABASE %I TO %I', current_database(), readonly_user);
    EXECUTE format('GRANT USAGE ON SCHEMA public TO %I', readonly_user);
    EXECUTE format('GRANT SELECT ON ALL TABLES IN SCHEMA public TO %I', readonly_user);
    EXECUTE format('ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT SELECT ON TABLES TO %I', readonly_user);

    EXECUTE format('CREATE USER %I WITH PASSWORD %L', readwrite_user, readwrite_password);
    EXECUTE format('GRANT CONNECT ON DATABASE %I TO %I', current_database(), readwrite_user);
    EXECUTE format('GRANT USAGE ON SCHEMA public TO %I', readwrite_user);
    EXECUTE format('GRANT SELECT, INSERT, UPDATE, DELETE ON ALL TABLES IN SCHEMA public TO %I', readwrite_user);
    EXECUTE format('GRANT USAGE, SELECT, UPDATE ON ALL SEQUENCES IN SCHEMA public TO %I', readwrite_user);
    EXECUTE format('ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT SELECT, INSERT, UPDATE, DELETE ON TABLES TO %I', readwrite_user);
    EXECUTE format('ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT USAGE, SELECT, UPDATE ON SEQUENCES TO %I', readwrite_user);
END $$;