import {
    createBrowserRouter
} from 'react-router-dom';

import App from './App';
import Home from './pages/Home/home.page';
import Projects from './pages/Project/projects.page';
import ProjectOptions from './pages/Project/project-options.page';
import TaskDetails from './components/Tasks/task-details.component';
import Error from './pages/Error/error.page';

const appRouter = createBrowserRouter([
    {
        path: '/',
        element: (<App />),
        children: [
            {
                path: "/",
                element: (<Home />),
            },
            {
                path: "/Projects",
                element: (<Projects />),
                children: [
                    {
                        path: '/Projects/:projectId',
                        element: (<ProjectOptions />),
                        children: [
                            {
                                path: '/Projects/:projectId/:taskId',
                                element: (<TaskDetails />),
                            }
                        ],
                    }
                ],
            }
        ],
    },
    {
        path: '*',
        element: (
            <Error>
                <h2>404 - Page Not Found</h2>
                <p>This is not the page you are looking for.</p>
            </Error>
        )
    }
]);

export default appRouter;