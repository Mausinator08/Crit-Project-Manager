import { JSX, useContext, useRef, useState } from 'react';

import { GetModuleContext } from '../../contexts/Module/module-context';
import { AuthService } from '../../services/AuthService.service';

type ProtectedRouteProps = {
    children: React.ReactNode;
};

function ProtectedRoute(props: ProtectedRouteProps): JSX.Element {
    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const moduleContext = useRef(GetModuleContext('app'));
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