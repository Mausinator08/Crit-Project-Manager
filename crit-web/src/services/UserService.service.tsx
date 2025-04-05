import { GetEnvValues } from "../constants/environment";
import { Injectable } from "../functions/Dependencies/injectable";
import { User } from "../models/user.model";

@Injectable()
export class UserService {
    private readonly userUrl: string =
        GetEnvValues()?.critApiUrl + "/User";

    public async GetUserByUserId(userId: string): Promise<User> {
        return new Promise(async (resolve, reject): Promise<void> => {
            const response = await fetch(new URL(`${this.userUrl}/${userId}`), {
                method: "GET",
                credentials: "include",
                mode: "cors",
                headers: {
                    "Content-Type": "application/json",
                },
            });

            if (response.status !== 200) {
                reject(await response.text());
                return;
            }

            const data: User = await response.json();
            resolve(data);
        });
    }

    public async GetUserByUserName(username: string): Promise<User> {
        return new Promise(async (resolve, reject): Promise<void> => {
            const response = await fetch(new URL(`${this.userUrl}/GetUserByUserName/${username}`), {
                method: "GET",
                credentials: "include",
                mode: "cors",
                headers: {
                    "Content-Type": "application/json",
                },
            });

            if (response.status !== 200) {
                reject(await response.text());
                return;
            }

            const data: User = await response.json();
            resolve(data);
        });
    }
}