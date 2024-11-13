import React, { createContext, useState } from "react";
import { Container } from "../dependencies/container";
import Error from "../pages/Error/error.page";
import { NavLink } from "react-router-dom";

interface ContextProps {
    getService: (service: any) => any;
}

export const ModuleContext = createContext<ContextProps>({
    getService: (service: any): any => {
        return null;
    },
});

interface Props {
    services: any[];
    children?: React.ReactNode;
}

const ModuleProvider: React.FC<Props> = ({ services, children }) => {
    const container: Container = new Container().init(services);

    const getService = (service: any): any => {
        try {
            return container.get(service);
        } catch (ex: any) {
            return (
                <Error>
                    <p style={{ color: 'red' }}>{(ex as Error).message}</p>
                    <NavLink to={'/Home'} key='return-home'>
                        Return to Home
                    </NavLink>
                </Error>
            );
        }
    }

    return (
        <ModuleContext.Provider
            value={{
                getService: getService,
            }}
        >
            {children}
        </ModuleContext.Provider>
    );
};

export default ModuleProvider;
