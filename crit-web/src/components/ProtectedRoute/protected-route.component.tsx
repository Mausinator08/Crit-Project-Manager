import { JSX, useState, useContext, useRef } from 'react';

import { GetModuleContext } from '../../contexts/Module/module-context';
import { AuthService } from '../../services/AuthService.service';

type ProtectedRouteProps = {
    children: React.ReactNode;
};

function ProtectedRoute(props: ProtectedRouteProps): JSX.Element {
    const moduleContext = useRef(GetModuleContext('app'));
    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const { getService } = useContext(moduleContext.current.context);
    const authService: AuthService = getService(AuthService);

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