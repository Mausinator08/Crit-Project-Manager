import {
    createBrowserRouter,
    RouteObject,
} from 'react-router-dom';
import { LinkChildren, links } from './constants/nav-bar-links';
import App from './App';

const appRouter = createBrowserRouter([
    {
        path: '/',
        element: (<App />),
        children: links.map<RouteObject>(LinkChildren),
    }
]);

export default appRouter;