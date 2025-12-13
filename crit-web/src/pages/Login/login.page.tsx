import { JSX } from "react";
import { GetEnvValues } from "../../constants/environment";
import { navigate } from "../../functions/Utils/navigation-utils";
import { User } from "../../models/requests/user.model";
import "./login.scss";
import { NavLink } from "react-router-dom";
import { UseService } from "../../contexts/Module/module-context";
import { AuthService } from "../../services/AuthService.service";

function Login(): JSX.Element {
    const authService: AuthService = UseService('app', AuthService);

    const submitLogin = (userName: string, password: string, useCookies: string): Promise<void> => {
        return new Promise<void>((resolve, reject) => {
            fetch(new URL(`${GetEnvValues()?.critApiUrl}/Login?useCookies=${useCookies}`), {
                method: "POST",
                mode: 'cors',
                body: JSON.stringify(new User(userName, password)),
                headers: {
                    "Content-Type": "application/json"
                },
                credentials: 'include',
            }).then(async (response) => {
                if (authService) {
                    authService.CheckAuthentication();
                }
                if (response.status === 200) {
                    const result = await response.json();
                    console.log(result.message);
                    resolve();
                    navigate(`/Home`);
                } else {
                    alert(await response.text());
                    resolve();
                }
            }).catch((error) => {
                reject(error);
            });
        });
    }

    return (
        <div className="login-form-body">
            <h2>Login</h2>
            <hr />
            <form onSubmit={(e) => {
                e.preventDefault();
                const username = (document.getElementById("username") as HTMLInputElement).value;
                const password = (document.getElementById("password") as HTMLInputElement).value;
                const useCookies = (document.getElementById("useCookies") as HTMLInputElement).checked;
                submitLogin(username, password, useCookies.toString());
            }
            }>
                <label>Username</label>
                <br />
                <input
                    type="text"
                    id="username"
                    name="username"
                    placeholder="Username" />
                <br />
                <label>Password</label>
                <br />
                <input
                    type="password"
                    id="password"
                    name="password"
                    placeholder="Password" />
                <br />
                <label htmlFor="useCookies" className="checkbox-label">
                    <input
                        type="checkbox"
                        id="useCookies"
                        name="useCookies"
                    />
                    <span className="checkbox-span">Remember Me</span>
                </label>
                <div className="login-form-grid">
                    <NavLink className="login-form-item" to={'/Register'}>Register</NavLink>
                    <NavLink className="login-form-item" to={'/Forgot-Password'}>Forgot Password</NavLink>
                    <button className="login-form-item" type="submit">Login</button>
                </div>
            </form>
        </div >
    );
}

export default Login;