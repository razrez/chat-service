
export async function getClaims(){
    const response = fetch(`${api}/auth/validate_token`, {
        method: "GET",
        headers: {
            'Access-Control-Allow-Origin': '*',
            'Authorization': `Bearer ${getToken()}`
        }
    }).then(response => response.json())
    return await response
}