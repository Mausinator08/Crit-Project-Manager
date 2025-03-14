import { JSX, useState, useEffect } from 'react';

import { CheckIsAuthenticated } from "../../functions/Auth/authentication";
import { navigate } from "../../functions/Utils/navigation-utils";

type ProtectedRouteProps = {
    children: React.ReactNode;
};

function ProtectedRoute(props: ProtectedRouteProps): JSX.Element {
    const [isAuthenticated, setIsAuthenticated] = useState(false);

    useEffect(() => {
        CheckIsAuthenticated().then((auth) => {
            setIsAuthenticated(auth.result);
            if (!auth) {
                navigate("/Login");
            }
        });
    }, []);

    return (
        <>
            {isAuthenticated && props.children}
        </>
    );

}

export default ProtectedRoute;