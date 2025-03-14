import { JSX, useContext } from "react";
import { GetEnvValues } from "../../constants/environment";
import { Button, Form } from "react-bootstrap";
import { navigate } from "../../functions/Utils/navigation-utils";
import { User } from "../../models/requests/user.model";
import "./login.scss";
import { NavLink } from "react-router-dom";
import { ModuleContext } from "../../contexts/Module/module-context";
import { AuthService } from "../../services/AuthService.service";

function Login(): JSX.Element {
    const { getService } = useContext(ModuleContext);
    const authService: AuthService = getService(AuthService);

    const submitLogin = (userName: string, password: string, useCookies: string, useSessionCookies: string): Promise<void> => {
        return new Promise<void>((resolve, reject) => {
            fetch(new URL(`${GetEnvValues()?.critApiUrl}/Login?useCookies=${useCookies}&useSessionCookies=${useSessionCookies}`), {
                method: "POST",
                mode: 'cors',
                body: JSON.stringify(new User(userName, password)),
                headers: {
                    "Content-Type": "application/json"
                },
                credentials: 'include',
            }).then(async (response) => {
                authService.CheckAuthentication();
                alert(await response.text());
                if (response.status === 200) {
                    resolve();
                    navigate(`/`);
                } else {
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
            <Form onSubmit={(e) => {
                e.preventDefault();
                const username = (document.getElementById("username") as HTMLInputElement).value;
                const password = (document.getElementById("password") as HTMLInputElement).value;
                const useCookies = (document.getElementById("useCookies") as HTMLInputElement).checked;
                const useSessionCookies = (document.getElementById("useSessionCookies") as HTMLInputElement).checked;
                submitLogin(username, password, useCookies.toString(), useSessionCookies.toString());
            }
            }>
                <Form.Group>
                    <Form.Label>Username</Form.Label>
                    <Form.Control
                        type="text"
                        id="username"
                        name="username"
                        placeholder="Username">
                    </Form.Control>
                    <Form.Label>Password</Form.Label>
                    <Form.Control
                        type="password"
                        id="password"
                        name="password"
                        placeholder="Password">
                    </Form.Control>
                </Form.Group>
                <Form.Group className="login-form-grid">
                    <Form.Label className="login-form-item">Use Cookies</Form.Label>
                    <Form.Check
                        className="login-form-item"
                        type="checkbox"
                        id="useCookies"
                        name="useCookies"
                    />
                    <Form.Label className="login-form-item">Use Session Cookies</Form.Label>
                    <Form.Check
                        className="login-form-item"
                        type="checkbox"
                        id="useSessionCookies"
                        name="useSessionCookies"
                    />
                </Form.Group>
                <Form.Group className="login-form-grid">
                    <Button className="login-form-button" variant="primary" type="submit">Login</Button>
                    <NavLink className="login-form-item" to={'/Register'}>Register</NavLink>
                    <NavLink className="login-form-item" to={'/Forgot-Password'}>Forgot Password</NavLink>
                </Form.Group>

            </Form>
        </div >
    );
}

export default Login;