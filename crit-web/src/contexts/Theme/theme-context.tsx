import React, { createContext, useState } from "react";

interface ThemeContextProps {
  theme: string;
  toggleTheme: () => void;
}

export const ThemeContext = createContext<ThemeContextProps>({
  theme: "light",
  toggleTheme: () => { },
});

interface ThemeProviderProps {
  children?: React.ReactNode;
}

const ThemeProvider: React.FC<ThemeProviderProps> = ({ children }) => {
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
    <ThemeContext
      value={{
        theme: theme,
        toggleTheme: toggleThemeHandler,
      }}
    >
      {children}
    </ThemeContext>
  );
};

export default ThemeProvider;
