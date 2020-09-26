import React, { createContext } from "react";

export const AuthenticationContext = createContext<{
    email: string | null;
    setEmail(email: string | null): void;
}>({
    email: null,
    setEmail: () => {},
});
