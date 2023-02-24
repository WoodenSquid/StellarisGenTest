import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { FetchCwData } from "./components/FetchCwData";
import { FetchSpeciesData } from "./components/FetchSpeciesData";
import { FetchEthicsData } from "./components/FetchEthicsData";
import { Home } from "./components/Home";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/counter',
    element: <Counter />
  },
  {
    path: '/fetch-data',
    element: <FetchData />
  },
  {
    path: '/fetch-CW-data',
    element: <FetchCwData />
  },
  {
    path: '/fetch-Species-Class-data',
    element: <FetchSpeciesData />
  },
  {
    path: '/fetch-Ethics-data',
    element: <FetchEthicsData />
  }
];

export default AppRoutes;
