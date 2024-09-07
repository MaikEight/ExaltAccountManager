import { defineConfig } from 'vite';
import { resolve } from 'path';

export default defineConfig({
  build: {
    lib: {
      entry: resolve(__dirname, 'src/index.js'),
      name: 'EamCommonsJs',
      fileName: (format) => `eam-commons-js.${format}.js`,
    },
    rollupOptions: {
      // Ensure to externalize dependencies that shouldn't be bundled
      external: [],
      output: {
        globals: {},
      },
    },
  },
});