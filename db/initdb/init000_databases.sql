CREATE USER docker;
CREATE DATABASE highloadsocial;
GRANT ALL PRIVILEGES ON DATABASE highloadsocial TO docker;

\c highloadsocial;

CREATE TABLE IF NOT EXISTS users (
    id text PRIMARY KEY,
    password_hash text NOT NULL,
    first_name text NOT NULL,
    second_name text NOT NULL,
    birthdate DATE NOT NULL,
    biography TEXT,
    city text
);


CREATE TABLE IF NOT EXISTS friendships (
    user_id text NOT NULL,
    friend_id text NOT NULL,
    PRIMARY KEY (user_id, friend_id),
    FOREIGN KEY (user_id) REFERENCES users(id),
    FOREIGN KEY (friend_id) REFERENCES users(id)
);

CREATE TABLE IF NOT EXISTS posts (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    text VARCHAR(1000) NOT NULL,
    user_id text NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (user_id) REFERENCES users(id)
);


\c postgres;
CREATE TABLE IF NOT EXISTS seed_status (
    id SERIAL PRIMARY KEY,
    seed_completed BOOLEAN NOT NULL,
    completed_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
