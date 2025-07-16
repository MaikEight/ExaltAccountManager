import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";
import { resolve } from 'path';

export default defineConfig(async () => ({
  plugins: [react()],

  // Vite options tailored for Tauri development and only applied in `tauri dev` or `tauri build`
  //
  // 1. prevent vite from obscuring rust errors
  clearScreen: false,
  optimizeDeps: {
    include: [
      '@mui/material/Tooltip',
      '@emotion/styled',
      '@mui/material',
      '@mui/x-data-grid',
    ],
    exclude: ['eam-commons-js'],
  },
  resolve: {
    alias: {
      'stream': 'stream-browserify',
    }
  },
  // 2. tauri expects a fixed port, fail if that port is not available
  server: {
    port: 1420,
    strictPort: true,
  }
}));