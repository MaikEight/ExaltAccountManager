import { defineConfig } from 'vite';
import { resolve } from 'path';
import react from "@vitejs/plugin-react";

const EXTERNAL_PREFIXES = ['react', '@emotion/', '@mui/'];
const EXTERNAL_EXACT = new Set([
  'react',
  'react-dom',
  'styled-components',
]);

function isExternal(id) {
  if (EXTERNAL_EXACT.has(id)) return true;
  return EXTERNAL_PREFIXES.some(prefix => id.startsWith(prefix));
}

export default defineConfig({
  plugins: [react()],
  build: {
    lib: {
      entry: resolve(__dirname, 'src/index.jsx'),
      name: 'EamCommonsJs',
      fileName: (format) => `eam-commons-js.${format}.js`,
    },
    rollupOptions: {
      external: isExternal,
      output: {
        globals: (id) => {
          if (id === 'react') return 'React';
          if (id === 'react-dom') return 'ReactDOM';
          if (id === 'react/jsx-runtime') return 'ReactJSXRuntime';
          if (id === 'react/jsx-dev-runtime') return 'ReactJSXDevRuntime';
          if (id.startsWith('@mui/')) return 'MaterialUI';
          if (id.startsWith('@emotion/')) return 'Emotion';
          return id;
        },
      },
    },
  },
  optimizeDeps: {
    include: ['styled-components'],
  },
});
