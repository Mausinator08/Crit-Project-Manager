import { JSX, useState } from 'react';

import { UseService } from '../../contexts/Module/module-context';
import { AuthService } from '../../services/AuthService.service';

type ProtectedRouteProps = {
    children: React.ReactNode;
};

function ProtectedRoute(props: ProtectedRouteProps): JSX.Element {
    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const authService: AuthService = UseService('app', AuthService);

    setInterval(() => {
        if (authService) {
            setIsAuthenticated(authService.IsAuthenticated());
        }
    });

    return (
        <>
            {isAuthenticated && props.children}
        </>
    );

}

export default ProtectedRoute;