import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";

import ExternalLinkSvg from "../../assets/svg/icons8-external-link-30.png";
import { NavItem } from "../../types/NavItem";
import styles from "./NavBar.module.css";

interface NavbarProps {
  logoSrc: string;
  navItems: NavItem[];
}

const Navbar: React.FC<NavbarProps> = ({ logoSrc, navItems }) => {
  const [isSticky, setIsSticky] = useState(false);

  useEffect(() => {
    const handleScroll = () => {
      const scrollPosition = window.scrollY;
      setIsSticky(scrollPosition > 0);
    };

    window.addEventListener("scroll", handleScroll);

    return () => {
      window.removeEventListener("scroll", handleScroll);
    };
  }, []);

  return (
    <nav
      className={`${styles.navbar} ${isSticky ? styles.sticky : ""} bg-dark`}
    >
      <div className={styles.navLeft}>
        <img src={logoSrc} className={styles.logo} alt="where imag" />

        <ul className={styles.menu}>
          {navItems.map((item, index) => (
            <li key={index} className={styles.menuItem}>
              <Link className={styles.menuLink} to={item.path}>
                {item.label}
              </Link>
            </li>
          ))}
        </ul>
      </div>
      <div className={styles.navRight}>
        <a
          href="https://github.com/futuramafamilyguy/personal-website"
          target="_blank"
          className={styles.externalLink}
        >
          GitHub
          <img src={ExternalLinkSvg} />
        </a>
      </div>
    </nav>
  );
};

export default Navbar;
