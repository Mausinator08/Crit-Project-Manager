import { CheckIsAuthenticated } from '../functions/Auth/authentication';
import { Injectable } from '../functions/Dependencies/injectable';

@Injectable()
export class AuthService {
    private isAuthenticated: boolean = false;

    private constructor() { }

    public IsAuthenticated(): boolean {
        return this.isAuthenticated;
    }

    public async CheckAuthentication(): Promise<void> {
        const result = await CheckIsAuthenticated();
        this.isAuthenticated = result.result;
    }
}