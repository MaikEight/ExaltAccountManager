import { ColorContextProvider } from 'eam-commons-js';
import MainProviders from './MainProviders';

function App() {

  return (
    <ColorContextProvider>
      <MainProviders />
    </ColorContextProvider>
  );
}

export default App;
