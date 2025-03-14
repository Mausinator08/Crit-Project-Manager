import {
    createBrowserRouter
} from 'react-router-dom';

import App from './App';
import Home from './pages/Home/home.page';
import Projects from './pages/Project/projects.page';
import ProjectOptions from './pages/Project/project-options.page';
import TaskDetails from './components/Tasks/task-details.component';
import Error from './pages/Error/error.page';
import Login from './pages/Login/login.page';
import ProtectedRoute from './components/ProtectedRoute/protected-route.component';
import { GetEnvValues } from './constants/environment';
import { navigate } from './functions/Utils/navigation-utils';
import Logout from './pages/Logout/logout.page';
import Register from './pages/Register/register.page';

const appRouter = createBrowserRouter([
    {
        path: '/',
        element: (<App />),
        children: [
            {
                path: "/Home",
                element: (<ProtectedRoute><Home /></ProtectedRoute>),
            },
            {
                path: "/Login",
                element: (<Login />)
            },
            {
                path: "/Register",
                element: (<Register />)
            },
            {
                path: "/Projects",
                element: (<ProtectedRoute><Projects /></ProtectedRoute>),
                children: [
                    {
                        path: '/Projects/:projectId',
                        element: (<ProtectedRoute><ProjectOptions /></ProtectedRoute>),
                        children: [
                            {
                                path: '/Projects/:projectId/:taskId',
                                element: (<ProtectedRoute><TaskDetails /></ProtectedRoute>),
                            }
                        ],
                    }
                ],
            },
            {
                path: '/Logout',
                element: (<Logout />),
            },
        ],
    },
    {
        path: '*',
        element: (
            <Error>
                <h2>404 - Page Not Found</h2>
                <p>This is not the page you are looking for.</p>
            </Error>
        ),
    }
]);

export default appRouter;