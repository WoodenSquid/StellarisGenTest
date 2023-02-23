import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { FetchCwData } from "./components/FetchCwData";
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
  }
];

export default AppRoutes;
