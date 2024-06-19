import { Link } from "react-router-dom";

import React, { useEffect, useState } from "react";
import styles from "./NavBar.module.css";
import { MenuItem } from "../../types/MenuItem";

interface NavbarProps {
  logoSrc: string;
  menuItems: MenuItem[];
}

const Navbar: React.FC<NavbarProps> = ({ logoSrc, menuItems }) => {
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
    <nav className={`${styles.navbar} ${isSticky ? styles.sticky : ""}`}>
      <img src={logoSrc} className={styles.logo} alt="Logo" />

      <ul className={styles.menu}>
        {menuItems.map((item, index) => (
          <li key={index} className={styles.menuItem}>
            <Link className={styles.menuLink} to={item.link}>
              {item.label}
            </Link>
          </li>
        ))}
      </ul>
    </nav>
  );
};

export default Navbar;
