interface ImportMetaEnv {
    readonly VITE_SERVER_URL: string;
    readonly VITE_OPERATION_KINO_URL: string;
}

interface ImportMeta {
    readonly env: ImportMetaEnv;
}