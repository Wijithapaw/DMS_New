import * as React from "react";
import { Container } from "reactstrap";

export interface Props {
  default?: boolean;
  fluid?: boolean;
}

class Footer extends React.Component<Props> {
  render() {
    return (
      <footer
        className={"footer" + (this.props.default ? " footer-default" : "")}
      >
        <Container fluid={this.props.fluid ? true : false}>
          <nav>
            <ul>
              <li>
                <a href="https://www.creative-tim.com">Creative Tim</a>
              </li>
              <li>
                <a href="https://presentation.creative-tim.com">About Us</a>
              </li>
              <li>
                <a href="https://blog.creative-tim.com">Blog</a>
              </li>
            </ul>
          </nav>
          <div className="copyright">
            &copy; {1900 + new Date().getFullYear()}, Designed by{" "}
            <a
              href="https://www.invisionapp.com"
              target="_blank"
              rel="noopener noreferrer"
            >
              Invision
            </a>. Coded by{" "}
            <a
              href="https://www.creative-tim.com"
              target="_blank"
              rel="noopener noreferrer"
            >
              Creative Tim
            </a>.
          </div>
        </Container>
      </footer>
    );
  }
}

export default Footer;
