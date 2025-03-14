import React, { createContext, useCallback, useEffect, useRef, useState } from "react";
import { Container } from "../../dependencies/container";
import Error from "../../pages/Error/error.page";
import { NavLink } from "react-router-dom";
import { Effect } from "../../functions/Utils/effect";

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

const defaultContainer = new Container();

const ModuleProvider: React.FC<Props> = ({ services, children }) => {
    const container = useRef<Container>(defaultContainer);
    const [isValidContainer, setIsValidContainer] = useState<boolean>(false);

    const initContainer = useCallback(() => {
        if (!container.current?.isInitialized) {
            container.current = defaultContainer.init(services);
            setIsValidContainer(true);
        }
    }, []);

    const getService = (service: any): any => {
        try {
            return container.current?.get(service);
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
        <ModuleContext
            value={{
                getService: getService,
            }}
        >
            <Effect callback={initContainer} />
            {isValidContainer && children}
        </ModuleContext>
    );
};

export default ModuleProvider;
