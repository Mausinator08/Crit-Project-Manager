
import { JSX, useEffect } from "react";
import { Outlet } from "react-router-dom";
import { navigate } from "../../functions/Utils/navigation-utils";
import { GetEnvValues } from "../../constants/environment";

function Logout(): JSX.Element {
    useEffect(() => {
        fetch(new URL(`${GetEnvValues()?.critApiUrl}/Logout`), {
            method: "POST",
            mode: 'cors',
            credentials: 'include',
        }).then(async (response) => {
            alert(await response.text());
            if (response.status === 200) {
                navigate(`/Login`);
            }
        });
    }, [navigate]);

    return (
        <p>Logout</p>
    );
}

export default Logout;


