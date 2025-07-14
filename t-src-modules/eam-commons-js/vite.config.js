import { defineConfig } from 'vite';
import { resolve } from 'path';
import react from "@vitejs/plugin-react";

export default defineConfig({
  plugins: [react()],
  build: {
    lib: {
      entry: resolve(__dirname, 'src/index.jsx'),
      name: 'EamCommonsJs',
      fileName: (format) => `eam-commons-js.${format}.js`,
    },
    rollupOptions: {
      external: ['react', 'react-dom', '@mui/material', 'styled-components'],
      output: {
        globals: {
          react: 'React',
          'react-dom': 'ReactDOM',
          '@mui/material': 'MaterialUI',
        },
      },
    },
  },
  resolve: {
    alias: {
      '@mui/material': resolve(__dirname, 'node_modules/@mui/material'),
    },
  },
  optimizeDeps: {
    include: ['@mui/material', 'styled-components'],
  },
});