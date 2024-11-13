import React, { createContext, useState } from "react";

interface ContextProps {
  theme: string;
  toggleTheme: () => void;
}

export const ThemeContext = createContext<ContextProps>({
  theme: "light",
  toggleTheme: () => { },
});

interface Props {
  children?: React.ReactNode;
}

const ThemeProvider: React.FC<Props> = ({ children }) => {
  const [theme, setTheme] = useState(localStorage.getItem('theme') ?? 'light');

  const toggleThemeHandler = () => {
    setTheme((prevState): string => {
      if (prevState === 'light') {
        return 'dark';
      } else if (prevState === 'dark') {
        return 'light';
      }

      return 'light';
    });
  };

  return (
    <ThemeContext.Provider
      value={{
        theme: theme,
        toggleTheme: toggleThemeHandler,
      }}
    >
      {children}
    </ThemeContext.Provider>
  );
};

export default ThemeProvider;
