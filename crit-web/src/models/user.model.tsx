export class User {
    constructor(username: string, password: string, email?: string, organization?: string, phonenumber?: string, countryCode?: string, extension?: string) {
        this.username = username;
        this.password = password;
        this.email = email;
        this.organization = organization;
        this.phonenumber = phonenumber;
        this.countryCode = countryCode;
        this.extension = extension;
    }

    public id?: string;
    public username: string;
    public password: string;
    public email?: string;
    public organization?: string;
    public phonenumber?: string;
    public countryCode?: string;
    public extension?: string;
}