
import { JSX, useContext, useEffect, useRef } from "react";
import { navigate } from "../../functions/Utils/navigation-utils";
import { GetEnvValues } from "../../constants/environment";
import { GetModuleContext } from "../../contexts/Module/module-context";
import { AuthService } from "../../services/AuthService.service";

function Logout(): JSX.Element {
    const moduleContext = useRef(GetModuleContext('app'));
    const { getService } = useContext(moduleContext.current.context);
    const authService: AuthService = getService(AuthService);

    useEffect(() => {
        fetch(new URL(`${GetEnvValues()?.critApiUrl}/Logout`), {
            method: "POST",
            mode: 'cors',
            credentials: 'include',
        }).then(async (response) => {
            if (response.status === 200) {
                console.log(await response.text());
                if (authService) {
                    authService.CheckAuthentication();
                }
                navigate(`/Login`);
            } else {
                alert(await response.text());
            }
        });
    }, []);

    return (
        <p>Logout</p>
    );
}

export default Logout;


