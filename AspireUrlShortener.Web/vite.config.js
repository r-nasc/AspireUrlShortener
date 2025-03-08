/* eslint-disable no-undef */
import { defineConfig, loadEnv } from 'vite';
import react from '@vitejs/plugin-react';

// https://vitejs.dev/config/
export default defineConfig(({ mode }) => {
    const env = loadEnv(mode, process.cwd(), '');

    const backend_url = process.env["services__urlshortener-api__https__0"] ||
        process.env["services__urlshortener-api__http__0"];

    return {
        plugins: [react()],
        server: {
            port: parseInt(env.VITE_PORT),
            proxy: {
                '/api': {
                    target: backend_url,
                    changeOrigin: true,
                    rewrite: (path) => '/api/v1/' + path.replace('/api/', ''),
                    secure: false,
                },
                '/r': {
                    target: backend_url,
                    changeOrigin: true,
                    rewrite: (path) => '/api/v1/' + path.replace('/r/', ''),
                    secure: false,
                }
            }
        },
        build: {
            outDir: 'dist',
            rollupOptions: {
                input: './index.html'
            }
        }
    }
})