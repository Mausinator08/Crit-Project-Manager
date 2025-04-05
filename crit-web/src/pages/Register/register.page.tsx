import { JSX, useState } from "react";
import { Button, Form } from "react-bootstrap";
import { NavLink } from "react-router-dom";
import { GetEnvValues } from "../../constants/environment";
import { navigate } from "../../functions/Utils/navigation-utils";
import { User } from "../../models/user.model";

import "./register.scss";
import PhoneInputWithCountrySelect, { isValidPhoneNumber, parsePhoneNumber, Value } from "react-phone-number-input";


function Register(): JSX.Element {
    const submitRegistration = (userName: string, password: string, email: string, organization: string, phonenumber?: string, countryCode?: string, extension?: string): Promise<void> => {
        return new Promise<void>((resolve, reject) => {
            fetch(new URL(`${GetEnvValues()?.critApiUrl}/Register`), {
                method: "POST",
                mode: 'cors',
                body: JSON.stringify(new User(userName, password, email, organization, phonenumber, countryCode, extension)),
                headers: {
                    "Content-Type": "application/json"
                },
                credentials: 'include',
            }).then(async (response) => {
                alert(await response.text());
                if (response.status === 200) {
                    resolve();
                    navigate(`/Login`);
                } else {
                    resolve();
                }
            }).catch((error) => {
                reject(error);
            });
        });
    }

    const [phonenumber, setPhonenumber] = useState<Value | undefined>(undefined);

    return (
        <div className="register-form-body">
            <h2>Login</h2>
            <hr />
            <Form onSubmit={(e) => {
                e.preventDefault();
                const username = (document.getElementById("username") as HTMLInputElement).value;
                const password = (document.getElementById("password") as HTMLInputElement).value;
                const email = (document.getElementById("email") as HTMLInputElement).value;
                const organization = (document.getElementById("organization") as HTMLInputElement).value;
                if (phonenumber && isValidPhoneNumber(phonenumber)) {
                    const parsedPhoneNumber = parsePhoneNumber(phonenumber);
                    submitRegistration(
                        username,
                        password,
                        email,
                        organization,
                        parsedPhoneNumber?.number,
                        parsedPhoneNumber?.country,
                        parsedPhoneNumber?.ext
                    );
                }
            }}>
                <Form.Group>
                    <Form.Label>Username</Form.Label>
                    <Form.Control
                        type="text"
                        id="username"
                        name="username"
                        placeholder="Username">
                    </Form.Control>
                    <Form.Label>Email</Form.Label>
                    <Form.Control
                        type="text"
                        id="email"
                        name="email"
                        placeholder="Email">
                    </Form.Control>
                    <Form.Label>Password</Form.Label>
                    <Form.Control
                        type="password"
                        id="password"
                        name="password"
                        placeholder="Password">
                    </Form.Control>
                    <Form.Label>Organization</Form.Label>
                    <Form.Control
                        type="text"
                        id="organization"
                        name="organization"
                        placeholder="Organization">
                    </Form.Control>
                    <Form.Label>PhoneNumber</Form.Label>
                    <PhoneInputWithCountrySelect
                        id="phonenumber"
                        name="phonenumber"
                        placeholder="Phone Number"
                        rules={{ required: true }}
                        onChange={function (value?: Value): void {
                            setPhonenumber(value);
                        }} />
                </Form.Group>
                <Form.Group className="register-form-grid">
                    <Button className="register-form-button" variant="primary" type="submit">Register</Button>
                    <NavLink className="register-form-item" to={'/Login'}>I have an account</NavLink>
                </Form.Group>

            </Form>
        </div >
    );
}

export default Register;