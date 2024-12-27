import {
    createBrowserRouter
} from 'react-router-dom';

import App from './App';
import Home from './pages/Home/home.page';
import Projects from './pages/Projects/projects.page';
import ProjectOptions from './pages/Project/project-options.page';
import TaskDetails from './components/TaskDetails/task-details.component';

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
]);

export default appRouter;