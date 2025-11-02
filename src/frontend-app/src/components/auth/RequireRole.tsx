import React from "react";
import { Navigate, useLocation } from "react-router-dom";
import { useUser } from "../context/userContext";
import { JWTManager } from "../../auth/jwt/jwt-manager";

type Props = {
    roles?: string[]; // allowed roles; if omitted -> any authenticated user
    children: React.ReactElement;
    redirectTo?: string; // when authenticated but not authorized
};

export default function RequireRole({ roles, children, redirectTo = "/dashboard/unauthorized" }: Props) {
    const { user, isLoading, userRoles } = useUser();
    const location = useLocation();

    // Show nothing while checking auth; the parent layout can include a spinner
    if (isLoading) return null;

    // const isAuthed = !!user && JWTManager.isAuthenticated();
    // if (!isAuthed) {
    //     return <Navigate to="/login" state={{ from: location }} replace />;
    // }

    if (!roles || roles.length === 0) {
        return children;
    }

    const rolesList = Array.isArray(userRoles) ? userRoles : (userRoles ? [userRoles] : []);
    const allowed = roles.some(r => rolesList.includes(r));

    return allowed ? children : <Navigate to={redirectTo} replace />;
}
