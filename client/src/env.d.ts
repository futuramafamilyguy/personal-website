interface ImportMetaEnv {
    readonly VITE_SERVER_URL: string;
    readonly VITE_SESSION_URL: string;
}

interface ImportMeta {
    readonly env: ImportMetaEnv;
}