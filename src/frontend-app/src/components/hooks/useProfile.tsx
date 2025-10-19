import { useUser } from "../context/userContext";

export function useProfile() {
    const { user, isLoading } = useUser();

    return {
        user,
        isLoading,
    };
}
