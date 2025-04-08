import { GetEnvValues } from "../constants/environment";
import { Injectable } from "../functions/Dependencies/injectable";
import { ApiResult } from "../models/responses/api-result.model";
import { User } from "../models/user.model";

@Injectable()
export class UserService {
    private readonly userUrl: string =
        GetEnvValues()?.critApiUrl + "/User";

    public async GetUserByUserId(userId: string): Promise<ApiResult<User | null>> {
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
                reject(await response.json() as ApiResult<User | null>);
                return;
            }

            const data: ApiResult<User | null> = await response.json() as ApiResult<User | null>;
            resolve(data);
        });
    }

    public async GetUserByUserName(username: string): Promise<ApiResult<User | null>> {
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
                reject(await response.json() as ApiResult<User | null>);
                return;
            }

            const data: ApiResult<User | null> = await response.json() as ApiResult<User | null>;
            resolve(data);
        });
    }

    public async IsUserIdInRole(userId: string, role: string): Promise<ApiResult<boolean | null>> {
        return new Promise(async (resolve, reject): Promise<void> => {
            const response = await fetch(new URL(`${this.userUrl}/IsUserIdInRole?userId=${userId}&role=${role}`), {
                method: "GET",
                credentials: "include",
                mode: "cors",
                headers: {
                    "Content-Type": "application/json",
                },
            });

            if (response.status !== 200) {
                reject(await response.json() as ApiResult<boolean | null>);
                return;
            }

            const data: ApiResult<boolean | null> = await response.json() as ApiResult<boolean | null>;
            resolve(data);
        });
    }

    public async GetLoggedInUserId(): Promise<ApiResult<string | null>> {
        return new Promise(async (resolve, reject): Promise<void> => {
            const response = await fetch(new URL(`${this.userUrl}/GetLoggedInUserId`), {
                method: "GET",
                credentials: "include",
                mode: "cors",
                headers: {
                    "Content-Type": "application/json",
                },
            });

            if (response.status !== 200) {
                reject(await response.json() as ApiResult<string | null>);
                return;
            }

            const data: ApiResult<string | null> = await response.json() as ApiResult<string | null>;
            resolve(data);
        });
    }
}