import { useUser } from "../context/userContext";

export function useProfile() {
    const { user } = useUser();

    return (<>
        div
    </>)
}