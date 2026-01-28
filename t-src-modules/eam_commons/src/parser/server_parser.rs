use roxmltree::Document;
use crate::models::Server;

/// Parse servers from XML char/list response.
/// 
/// Extracts server information from the `<Servers>` node if present.
/// Returns empty Vec if `<Servers>` node is not found (e.g., in account/verify responses).
pub fn parse_servers(xml: &str) -> Vec<Server> {
    let doc = match Document::parse(xml) {
        Ok(doc) => doc,
        Err(_) => return vec![],
    };

    let mut servers = Vec::new();

    // Find the <Servers> node under <Chars>
    let servers_node = doc.descendants()
        .find(|n| n.has_tag_name("Servers"));

    if let Some(servers_parent) = servers_node {
        for server_node in servers_parent.children().filter(|n| n.has_tag_name("Server")) {
            let name = server_node.children()
                .find(|n| n.has_tag_name("Name"))
                .and_then(|n| n.text())
                .unwrap_or("")
                .to_string();

            let dns = server_node.children()
                .find(|n| n.has_tag_name("DNS"))
                .and_then(|n| n.text())
                .unwrap_or("")
                .to_string();

            let lat = server_node.children()
                .find(|n| n.has_tag_name("Lat"))
                .and_then(|n| n.text())
                .map(|s| s.to_string());

            let long = server_node.children()
                .find(|n| n.has_tag_name("Long"))
                .and_then(|n| n.text())
                .map(|s| s.to_string());

            let usage = server_node.children()
                .find(|n| n.has_tag_name("Usage"))
                .and_then(|n| n.text())
                .map(|s| s.to_string());

            // Only add server if it has at least a name and dns
            if !name.is_empty() && !dns.is_empty() {
                servers.push(Server {
                    id: None,
                    name,
                    dns,
                    lat,
                    long,
                    usage,
                });
            }
        }
    }

    servers
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_parse_servers_from_char_list() {
        let xml = r#"<?xml version="1.0"?>
        <Chars>
            <Servers>
                <Server>
                    <Name>USWest</Name>
                    <DNS>uswest.server.com</DNS>
                    <Lat>37.7749</Lat>
                    <Long>-122.4194</Long>
                    <Usage>0.5</Usage>
                </Server>
                <Server>
                    <Name>EUWest</Name>
                    <DNS>euwest.server.com</DNS>
                    <Lat>51.5074</Lat>
                    <Long>-0.1278</Long>
                </Server>
            </Servers>
        </Chars>"#;

        let servers = parse_servers(xml);
        assert_eq!(servers.len(), 2);
        assert_eq!(servers[0].name, "USWest");
        assert_eq!(servers[0].dns, "uswest.server.com");
        assert_eq!(servers[0].lat, Some("37.7749".to_string()));
        assert_eq!(servers[0].usage, Some("0.5".to_string()));
        assert_eq!(servers[1].name, "EUWest");
        assert_eq!(servers[1].usage, None);
    }

    #[test]
    fn test_parse_servers_no_servers_node() {
        let xml = r#"<?xml version="1.0"?>
        <Chars>
            <Account>
                <Name>TestAccount</Name>
            </Account>
        </Chars>"#;

        let servers = parse_servers(xml);
        assert!(servers.is_empty());
    }

    #[test]
    fn test_parse_servers_invalid_xml() {
        let xml = "not valid xml";
        let servers = parse_servers(xml);
        assert!(servers.is_empty());
    }
}
