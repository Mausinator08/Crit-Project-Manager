import { GetEnvValues } from "../../constants/environment";
import { navigate } from "../Utils/navigation-utils";


export const CheckIsAuthenticated = async (): Promise<{ result: boolean, message: string }> => {
    return new Promise<{ result: boolean, message: string }>(async (resolve, reject): Promise<void> => {
        const response = await fetch(new URL(`${GetEnvValues()?.critApiUrl}/IsAuthenticated`), {
            method: 'GET',
            mode: 'cors',
            credentials: 'include',
        });
        const result = await response.json();
        if (response.ok) {
            resolve({ result: result.authenticated, message: result.message });
            if (!result.authenticated) {
                navigate('/Login');
            }
        } else {
            resolve({ result: result.authenticated, message: result.message });
            navigate('/Login');
        }
    });
};