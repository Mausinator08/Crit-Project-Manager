import { GetEnvValues } from "../constants/environment";
import { Injectable } from "../functions/Dependencies/injectable";
import { Status } from "../models/status.model";

@Injectable()
export class StatusService {
    private readonly statusUrl: string =
        GetEnvValues()?.critApiUrl + "/Status";

    private constructor() { }

    public async GetAllStatuses(projectId: string): Promise<Status[]> {
        return new Promise<Status[]>(async (resolve, reject) => {
            const response = await fetch(new URL(`${this.statusUrl}/GetAllStatuses/${projectId}`), {
                method: "GET",
                mode: "cors",
                credentials: "include",
                headers: {
                    "Content-Type": "application/json",
                },
            });

            if (!response.ok) {
                reject(new Error(await response.text()));
                return;
            }

            const statuses: Status[] = await response.json();
            resolve(statuses);
        });
    }

    public async GetStatus(statusId: string): Promise<Status> {
        return new Promise<Status>(async (resolve, reject) => {
            const response = await fetch(new URL(`${this.statusUrl}/${statusId}`), {
                method: "GET",
                mode: "cors",
                credentials: "include",
                headers: {
                    "Content-Type": "application/json",
                },
            });

            if (!response.ok) {
                reject(new Error(await response.text()));
                return;
            }

            const status: Status = await response.json();
            resolve(status);
        });
    }

    public async CreateStatus(status: Status): Promise<Status> {
        return new Promise<Status>(async (resolve, reject) => {
            const response = await fetch(new URL(`${this.statusUrl}`), {
                method: "POST",
                mode: "cors",
                credentials: "include",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(status),
            });

            if (!response.ok) {
                reject(new Error(await response.text()));
                return;
            }

            const createdStatus: Status = await response.json();
            resolve(createdStatus);
        });
    }

    public async UpdateStatus(status: Status): Promise<Status> {
        return new Promise<Status>(async (resolve, reject) => {
            const response = await fetch(new URL(`${this.statusUrl}`), {
                method: "PUT",
                mode: "cors",
                credentials: "include",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(status),
            });

            if (!response.ok) {
                reject(new Error(await response.text()));
                return;
            }

            const updatedStatus: Status = await response.json();
            resolve(updatedStatus);
        });
    }

    public async DeleteStatus(statusId: string): Promise<void> {
        return new Promise<void>(async (resolve, reject) => {
            const response = await fetch(new URL(`${this.statusUrl}/${statusId}`), {
                method: "DELETE",
                mode: "cors",
                credentials: "include",
                headers: {
                    "Content-Type": "application/json",
                },
            });

            if (!response.ok) {
                reject(new Error(await response.text()));
                return;
            }

            resolve();
        });
    }
}