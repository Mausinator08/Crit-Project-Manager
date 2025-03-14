import 'process';

export type DevelopmentLocalEnv = {
    critApiUrl: string;
};

const developmentLocalEnv: DevelopmentLocalEnv = {
    critApiUrl: process.env.REACT_APP_CRIT_API_URL ?? "",
};

export function GetEnvValues(): DevelopmentLocalEnv | null {
    if (process.env.NODE_ENV === "development") {
        return developmentLocalEnv;
    }

    return null;
}
